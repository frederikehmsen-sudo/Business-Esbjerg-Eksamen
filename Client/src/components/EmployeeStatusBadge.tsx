import type { EmployeeResponse } from "@/api/Api";
import { Badge } from "@/components/ui";
import { formatDate, todayIso } from "@/lib/format";

export function EmployeeStatusBadge({ employee }: { employee: EmployeeResponse }) {
    if (employee.isActive) return <Badge tone="success">Active</Badge>;

    // "YYYY-MM-DD" strings compare correctly as plain text
    if (employee.startDate && employee.startDate > todayIso()) {
        return <Badge>Starts {formatDate(employee.startDate)}</Badge>;
    }

    return <Badge>Ended</Badge>;
}