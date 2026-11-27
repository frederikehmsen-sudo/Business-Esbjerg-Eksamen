import type { WeeklyHoursRequest } from "@/api/Api";
import { fieldClass } from "@/components/ui";

export const weekdays = [
    { key: "monday", label: "Monday" },
    { key: "tuesday", label: "Tuesday" },
    { key: "wednesday", label: "Wednesday" },
    { key: "thursday", label: "Thursday" },
    { key: "friday", label: "Friday" },
] as const;

export type WeekdayKey = (typeof weekdays)[number]["key"];
export type DayHours = { from: string; to: string }; // "HH:mm", or "" for a day off
export type WeekSchedule = Record<WeekdayKey, DayHours>;

const DEFAULT_DAY: DayHours = { from: "08:00", to: "16:00" };
const DAY_OFF: DayHours = { from: "", to: "" };

export function defaultWeek(): WeekSchedule {
    return {
        monday: { ...DEFAULT_DAY },
        tuesday: { ...DEFAULT_DAY },
        wednesday: { ...DEFAULT_DAY },
        thursday: { ...DEFAULT_DAY },
        friday: { ...DEFAULT_DAY },
    };
}

function minutes(time: string): number {
    const [h, m] = time.split(":").map(Number);
    return h * 60 + m;
}

export function hoursFor(day: DayHours): number {
    if (!day.from || !day.to) return 0;
    return Math.max(0, (minutes(day.to) - minutes(day.from)) / 60);
}

/** Returns an error message, or null if the week is valid. */
export function validateWeek(week: WeekSchedule): string | null {
    for (const { key, label } of weekdays) {
        const { from, to } = week[key];
        if (!from && !to) continue;
        if (!from || !to) return `${label}: fill in both "from" and "to", or mark it as a day off.`;
        if (minutes(to) <= minutes(from)) return `${label}: "to" must be later than "from".`;
    }
    return null;
}

/** Converts the form state to the shape the API expects. */
export function toWeeklyHoursRequest(week: WeekSchedule): WeeklyHoursRequest {
    const time = (value: string) => (value ? `${value}:00` : null);
    const request: Record<string, string | null> = {};
    for (const { key } of weekdays) {
        request[`${key}From`] = time(week[key].from);
        request[`${key}To`] = time(week[key].to);
    }
    return request as WeeklyHoursRequest;
}

export function WeeklyScheduleFields({
                                         value,
                                         onChange,
                                         disabled,
                                     }: {
    value: WeekSchedule;
    onChange: (week: WeekSchedule) => void;
    disabled?: boolean;
}) {
    function setDay(key: WeekdayKey, day: DayHours) {
        onChange({ ...value, [key]: day });
    }

    const total = weekdays.reduce((sum, d) => sum + hoursFor(value[d.key]), 0);

    return (
        <div className="overflow-x-auto">
            <table className="w-full text-sm">
                <thead>
                <tr className="border-b border-line text-left text-xs uppercase tracking-wide text-brand-hover">
                    <th className="py-2 pr-4 font-semibold">Day</th>
                    <th className="py-2 pr-4 font-semibold">Working</th>
                    <th className="py-2 pr-4 font-semibold">From</th>
                    <th className="py-2 pr-4 font-semibold">To</th>
                    <th className="py-2 text-right font-semibold">Hours</th>
                </tr>
                </thead>
                <tbody>
                {weekdays.map(({ key, label }) => {
                    const day = value[key];
                    const working = day.from !== "" || day.to !== "";
                    return (
                        <tr key={key} className="border-b border-line/60">
                            <td className="py-2 pr-4 font-semibold">{label}</td>
                            <td className="py-2 pr-4">
                                <input
                                    type="checkbox"
                                    aria-label={`${label} is a working day`}
                                    checked={working}
                                    disabled={disabled}
                                    onChange={e => setDay(key, e.target.checked ? { ...DEFAULT_DAY } : { ...DAY_OFF })}
                                    className="size-4 accent-brand-hover"
                                />
                            </td>
                            <td className="py-2 pr-4">
                                <div className="w-32">
                                    <input
                                        type="time"
                                        aria-label={`${label} from`}
                                        value={day.from}
                                        disabled={disabled || !working}
                                        onChange={e => setDay(key, { ...day, from: e.target.value })}
                                        className={fieldClass}
                                    />
                                </div>
                            </td>
                            <td className="py-2 pr-4">
                                <div className="w-32">
                                    <input
                                        type="time"
                                        aria-label={`${label} to`}
                                        value={day.to}
                                        disabled={disabled || !working}
                                        onChange={e => setDay(key, { ...day, to: e.target.value })}
                                        className={fieldClass}
                                    />
                                </div>
                            </td>
                            <td className="py-2 text-right tabular-nums">{hoursFor(day).toFixed(2)}</td>
                        </tr>
                    );
                })}
                </tbody>
                <tfoot>
                <tr>
                    <td colSpan={4} className="pt-3 font-semibold">Total per week</td>
                    <td className="pt-3 text-right font-semibold tabular-nums">{total.toFixed(2)}</td>
                </tr>
                </tfoot>
            </table>
        </div>
    );
}