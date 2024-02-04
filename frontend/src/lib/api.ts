const baseUrl = 'http://localhost:5293';

type ApiResponse<T> = SuccessApiResponse<T> | ApiError

type SuccessApiResponse<T> = {
    failed: false;
    data: T;
    statusCode: number;
}

type ApiError = HttpErrorApiResponse | UnexpectedErrorApiResponse;

type HttpErrorApiResponse = {
    failed: true;
    errorType: "http";
    error: Response
}

type UnexpectedErrorApiResponse = {
    failed: true;
    errorType: "unexpected";
    error: unknown;
}

type Path = string | URL;
type RequestOptions = RequestInit | undefined;

async function fetchJson<T>(path: Path, options?: RequestOptions): Promise<ApiResponse<T>> {
    try {
        const url = new URL(path, baseUrl)
        const response = await fetch(url, options);
        if (!response.ok) {
            return {
                failed: true,
                errorType: "http",
                error: response
            };
        }
        const data = await response.json();
        return {
            failed: false,
            data,
            statusCode: response.status
        };
    } catch (error) {
        return {
            failed: true,
            errorType: "unexpected",
            error
        };
    }
}

async function get<T>(path: string): Promise<ApiResponse<T>> {
    return fetchJson<T>(path);
}

async function post<T>(path: string, body: any): Promise<ApiResponse<T>> {
    return fetchJson<T>(path, toJsonRequestOptions("POST", body));
}

async function put<T>(path: string, body: any): Promise<ApiResponse<T>> {
    return fetchJson<T>(path, toJsonRequestOptions("PUT", body));
}

function toJsonRequestOptions(method: "POST" | "PUT", body: any): RequestInit {
    return {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
    };
}


export type SetSpotifyCredentialsRequest = {
    clientId: string;
    clientSecret: string;
    redirectUri: string;
}
type SetSpotifyCredentialsResponse = {
    authorizationUri: string;
}
export async function setSpotifyCredentials(request: SetSpotifyCredentialsRequest) {
    return await put<SetSpotifyCredentialsResponse>('/spotify/credentials', request);
}