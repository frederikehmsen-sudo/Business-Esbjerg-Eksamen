import {
    useId,
    type ButtonHTMLAttributes,
    type InputHTMLAttributes,
    type ReactNode,
    type SelectHTMLAttributes,
} from "react";

export function cx(...classes: (string | false | null | undefined)[]) {
    return classes.filter(Boolean).join(" ");
}

// Buttons

type ButtonVariant = "primary" | "secondary" | "danger";

const buttonVariants: Record<ButtonVariant, string> = {
    primary: "bg-brand text-surface hover:bg-brand-hover",
    secondary: "border-2 border-brand text-brand hover:bg-brand/10",
    danger: "bg-danger text-surface hover:bg-danger/85",
};

export function Button({
                           variant = "primary",
                           type = "button",
                           className,
                           ...props
                       }: ButtonHTMLAttributes<HTMLButtonElement> & { variant?: ButtonVariant }) {
    return (
        <button
            type={type}
            className={cx(
                "inline-flex items-center justify-center gap-2 rounded-lg px-4 py-2 text-sm font-bold",
                "cursor-pointer transition disabled:cursor-not-allowed disabled:opacity-60",
                buttonVariants[variant],
                className,
            )}
            {...props}
        />
    );
}

// Form Fields

export const fieldClass =
    "w-full rounded-lg border-2 border-line bg-surface px-3 py-2 text-sm text-brand " +
    "outline-none transition focus:border-brand-hover disabled:opacity-60";

export function Input({
                          label,
                          hint,
                          id,
                          className,
                          ...props
                      }: InputHTMLAttributes<HTMLInputElement> & { label?: string; hint?: string }) {
    const autoId = useId();
    const inputId = id ?? autoId;
    const hintId = hint ? `${inputId}-hint` : undefined;
    return (
        <div className="flex flex-col gap-1 text-left">
            {label && (
                <label htmlFor={inputId} className="text-sm font-semibold">
                    {label}
                </label>
            )}
            <input id={inputId} aria-describedby={hintId} className={cx(fieldClass, className)} {...props} />
            {hint && (
                <p id={hintId} className="text-xs text-white/50">
                    {hint}
                </p>
            )}
        </div>
    );
}

export function Select({
                           label,
                           id,
                           className,
                           children,
                           ...props
                       }: SelectHTMLAttributes<HTMLSelectElement> & { label?: string }) {
    const autoId = useId();
    const selectId = id ?? autoId;
    return (
        <div className="flex flex-col gap-1 text-left">
            {label && (
                <label htmlFor={selectId} className="text-sm font-semibold">
                    {label}
                </label>
            )}
            <select id={selectId} className={cx(fieldClass, className)} {...props}>
                {children}
            </select>
        </div>
    );
}

// Cards

export function Card({
                         title,
                         actions,
                         className,
                         children,
                     }: {
    title?: string;
    actions?: ReactNode;
    className?: string;
    children: ReactNode;
}) {
    return (
        <section className={cx("rounded-xl border border-line bg-surface p-6 text-left", className)}>
            {(title || actions) && (
                <div className="mb-4 flex items-center justify-between gap-4">
                    {title && <h2 className="text-lg font-bold text-brand">{title}</h2>}
                    {actions}
                </div>
            )}
            {children}
        </section>
    );
}

// Alerts

export function Alert({
                          kind = "error",
                          children,
                      }: {
    kind?: "error" | "success";
    children?: ReactNode;
}) {
    if (!children) return null;
    return (
        <div
            role={kind === "error" ? "alert" : "status"}
            className={cx(
                "rounded-lg border px-4 py-3 text-left text-sm",
                kind === "error" && "border-danger/40 bg-danger/10 text-danger",
                kind === "success" && "border-success/40 bg-success/10 text-success",
            )}
        >
            {children}
        </div>
    );
}
// PageHeaders
export function PageHeader({
                               title,
                               description,
                               actions,
                           }: {
    title: string;
    description?: string;
    actions?: ReactNode;
}) {
    return (
        <div className="mb-6 flex flex-wrap items-end justify-between gap-4">
            <div>
                <h1 className="text-2xl font-bold text-brand">{title}</h1>
                {description && <p className="mt-1 text-sm text-white/60">{description}</p>}
            </div>
            {actions && <div className="flex gap-2">{actions}</div>}
        </div>
    );
}

// Badges

export function Badge({
                          tone = "muted",
                          children,
                      }: {
    tone?: "success" | "muted";
    children: ReactNode;
}) {
    return (
        <span
            className={cx(
                "inline-block rounded-md border px-2 py-0.5 text-xs font-semibold",
                tone === "success" ? "border-success/40 bg-success/10 text-success" : "border-line text-white/60",
            )}
        >
            {children}
        </span>
    );
}