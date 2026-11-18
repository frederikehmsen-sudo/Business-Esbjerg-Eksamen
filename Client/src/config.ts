const { protocol, hostname } = window.location;

export const API_URL = hostname.endsWith(".fly.dev")
    ? "https://business-esbjerg-eksamen.fly.dev"
    : `${protocol}//${hostname}:5209`;