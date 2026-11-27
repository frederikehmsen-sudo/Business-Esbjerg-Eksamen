import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { api } from "@/api/client";
import type { EmployeeResponse } from "@/api/Api";
import { getErrorMessage } from "@/api/errors";
import { Alert, Button, Card, PageHeader } from "@/components/ui";
import { employmentTypeLabel, formatDate } from "@/lib/format";
import { EmployeeStatusBadge } from "@/components/EmployeeStatusBadge";

export default function EmployeesPage() {
    const navigate = useNavigate();
    const location = useLocation();
    const createdName = (location.state as { created?: string } | null)?.created;

    const [employees, setEmployees] = useState<EmployeeResponse[]>([]);
    const [includeFormer, setIncludeFormer] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        let cancelled = false;
        setLoading(true);
        setError("");

        api.api
            .employeeList({ includeFormer })
            .then(res => {
                if (!cancelled) setEmployees(res.data);
            })
            .catch(async err => {
                if (!cancelled) setError(await getErrorMessage(err));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [includeFormer]);

    const sorted = [...employees].sort((a, b) => (a.name ?? "").localeCompare(b.name ?? ""));

    return (
        <>
            <PageHeader
                title="Employees"
                description="Create employees and manage their employment and work schedules."
                actions={<Button onClick={() => navigate("/admin/employees/new")}>New employee</Button>}
            />

            <div className="mb-4 space-y-3">
                {createdName && <Alert kind="success">{createdName} was created.</Alert>}
                <Alert>{error}</Alert>
            </div>

            <Card>
                <div className="mb-4 flex items-center justify-between gap-4">
                    <label className="flex items-center gap-2 text-sm text-white/70">
                        <input
                            type="checkbox"
                            checked={includeFormer}
                            onChange={e => setIncludeFormer(e.target.checked)}
                            className="size-4 accent-brand-hover"
                        />
                        Show former employees
                    </label>
                    <span className="text-sm text-white/50">{sorted.length} shown</span>
                </div>

                {loading && sorted.length === 0 ? (
                    <p className="py-6 text-sm text-white/60">Loading employees…</p>
                ) : sorted.length === 0 ? (
                    <p className="py-6 text-sm text-white/60">No employees yet. Create the first one with "New employee".</p>
                ) : (
                    <div className="overflow-x-auto">
                        <table className="w-full text-sm">
                            <thead>
                            <tr className="border-b border-line text-left text-xs uppercase tracking-wide text-brand-hover">
                                <th className="py-2 pr-4 font-semibold">Name</th>
                                <th className="py-2 pr-4 font-semibold">Email</th>
                                <th className="py-2 pr-4 font-semibold">Type</th>
                                <th className="py-2 pr-4 font-semibold">Start</th>
                                <th className="py-2 pr-4 font-semibold">End</th>
                                <th className="py-2 font-semibold">Status</th>
                            </tr>
                            </thead>
                            <tbody>
                            {sorted.map(e => (
                                <tr key={e.id} className="border-b border-line/60 last:border-0 hover:bg-white/5">
                                    <td className="py-3 pr-4">
                                        <Link to={`/admin/employees/${e.id}`} className="font-semibold text-brand hover:underline">
                                            {e.name}
                                        </Link>
                                    </td>
                                    <td className="py-3 pr-4 text-white/80">{e.email}</td>
                                    <td className="py-3 pr-4">{employmentTypeLabel(e.employmentType)}</td>
                                    <td className="py-3 pr-4 tabular-nums">{formatDate(e.startDate)}</td>
                                    <td className="py-3 pr-4 tabular-nums">{formatDate(e.endDate)}</td>
                                    <td className="py-3">
                                        <EmployeeStatusBadge employee={e} />
                                    </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </Card>
        </>
    );
}