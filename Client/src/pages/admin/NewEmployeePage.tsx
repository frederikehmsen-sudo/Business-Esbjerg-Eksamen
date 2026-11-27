import { useState, type FormEvent } from "react";
import { Link, useNavigate } from "react-router-dom";
import { api } from "@/api/client";
import { EmploymentType } from "@/api/Api";
import { getErrorMessage } from "@/api/errors";
import { Alert, Button, Card, Input, PageHeader, Select } from "@/components/ui";
import {
    WeeklyScheduleFields,
    defaultWeek,
    toWeeklyHoursRequest,
    validateWeek,
} from "@/components/WeeklyScheduleFields";
import { todayIso } from "@/lib/format";

export default function NewEmployeePage() {
    const navigate = useNavigate();

    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [startDate, setStartDate] = useState(todayIso());
    const [employmentType, setEmploymentType] = useState<EmploymentType>(EmploymentType.FullTime);
    const [password, setPassword] = useState("");
    const [week, setWeek] = useState(defaultWeek());

    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");

    async function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        if (saving) return;

        const weekError = validateWeek(week);
        if (weekError) {
            setError(weekError);
            return;
        }

        setSaving(true);
        setError("");
        try {
            await api.api.employeeCreate({
                name,
                email,
                startDate,
                employmentType,
                initialPassword: password,
                workSchedule: toWeeklyHoursRequest(week),
            });
            navigate("/admin/employees", { state: { created: name } });
        } catch (err) {
            setError(await getErrorMessage(err));
            setSaving(false);
        }
    }

    return (
        <>
            <Link to="/admin/employees" className="mb-4 inline-block text-sm text-white/60 hover:text-white">
                ← Back to employees
            </Link>
            <PageHeader title="New employee" description="Creates the employee, their login and their first work schedule." />

            <form onSubmit={handleSubmit} className="space-y-6">
                <Card title="Details">
                    <div className="grid gap-4 sm:grid-cols-2">
                        <Input label="Name" value={name} onChange={e => setName(e.target.value)} disabled={saving} required />
                        <Input
                            label="Email"
                            type="email"
                            placeholder="name@businessesbjerg.com"
                            hint="Also the employee's username when logging in."
                            value={email}
                            onChange={e => setEmail(e.target.value)}
                            disabled={saving}
                            required
                        />
                        <Input
                            label="Start date"
                            type="date"
                            value={startDate}
                            onChange={e => setStartDate(e.target.value)}
                            disabled={saving}
                            required
                        />
                        <Select
                            label="Employment type"
                            value={employmentType}
                            onChange={e => setEmploymentType(e.target.value as EmploymentType)}
                            disabled={saving}
                        >
                            <option value={EmploymentType.FullTime}>Full time</option>
                            <option value={EmploymentType.PartTime}>Part time</option>
                        </Select>
                        <Input
                            label="Initial password"
                            type="password"
                            autoComplete="new-password"
                            hint="At least 8 characters. Give it to the employee."
                            minLength={8}
                            value={password}
                            onChange={e => setPassword(e.target.value)}
                            disabled={saving}
                            required
                        />
                    </div>
                </Card>

                <Card title="Weekly schedule">
                    <p className="mb-4 text-sm text-white/60">Starts on the employment start date.</p>
                    <WeeklyScheduleFields value={week} onChange={setWeek} disabled={saving} />
                </Card>

                <Alert>{error}</Alert>

                <div className="flex justify-end gap-2">
                    <Button variant="secondary" onClick={() => navigate("/admin/employees")} disabled={saving}>
                        Cancel
                    </Button>
                    <Button type="submit" disabled={saving}>
                        {saving ? "Creating…" : "Create employee"}
                    </Button>
                </div>
            </form>
        </>
    );
}