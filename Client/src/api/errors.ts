/** Turns an error from an API call into a message we can show the user. */
export async function getErrorMessage(err: unknown): Promise<string> {
    if (err instanceof Response) {
        const text = await err.text().catch(() => "");
        if (text) {
            try {
                const json = JSON.parse(text);
                // ASP.NET validation errors: { errors: { Email: ["..."] } }
                if (json?.errors) return Object.values(json.errors).flat().join(" ");
                return json?.title ?? text;
            } catch {
                return text; // plain-text message from our controllers
            }
        }
        return `Request failed (${err.status}).`;
    }
    return "Could not reach the server. Please try again.";
}