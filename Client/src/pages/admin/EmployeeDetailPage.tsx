import { Link, useParams } from "react-router-dom";
import { Card, PageHeader } from "@/components/ui";

export default function EmployeeDetailPage() {
    const { id } = useParams();

    return (
        <>
            <Link to="/admin/employees" className="mb-4 inline-block text-sm text-white/60 hover:text-white">
                ← Back to employees
            </Link>
            <PageHeader title="Employee" description={`Id: ${id}`} />
            <Card>
                <p className="text-sm text-white/60">Coming soon...</p>
            </Card>
        </>
    );
}