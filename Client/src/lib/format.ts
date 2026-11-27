// Today's date as "YYYY-MM-DD"
export function todayIso(): string {
    return new Date().toLocaleDateString("sv-SE");
}

/** "2026-11-27" -> "27-11-2026". Splits the string instead of using Date, so timezones can't shift the day. */
export function formatDate(iso?: string | null): string {
    if (!iso) return "—";
    const [year, month, day] = iso.split("-");
    return `${day}-${month}-${year}`;
}

export function employmentTypeLabel(type?: string | null): string {
    if (type === "FullTime") return "Full time";
    if (type === "PartTime") return "Part time";
    return type ?? "—";
}