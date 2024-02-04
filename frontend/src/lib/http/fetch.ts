const baseUrl = 'http://localhost:5293';

type ApiResponse = Success | ApiError;
type ApiResponseWithData<T> = SuccessWithData<T> | ApiError;

type Success = {
    ok: true;
    httpResponse: Response;
}

type SuccessWithData<T> = {
    ok: true;
    data: T;
    httpResponse: Response;
}

type ApiError = HttpError | UnexpectedError

type HttpError = {
    ok: false;
    errorType: "http";
    error: Response;
}

type UnexpectedError = {
    ok: false;
    errorType: "unexpected";
    error: unknown;
}


async function fetchResponse(path: string, options?: RequestInit): Promise<ApiResponse> {
    try {
        const url = new URL(path, baseUrl)
        const response = await fetch(url, options);
        if (!response.ok) {
            return {
                ok: false,
                errorType: "http",
                error: response
            }
        }
        return {
            ok: true,
            httpResponse: response
        }
    } catch (error) {
        return unexpectedError(error);
    }
}

async function fetchJson<T>(path: string, options?: RequestInit): Promise<ApiResponseWithData<T>> {
    try {
        const response = await fetchResponse(path, options);
        if (!response.ok) {
            return response;
        }

        const data = await response.httpResponse.json();
        return {
            ok: true,
            data,
            httpResponse: response.httpResponse
        }

    } catch (error) {
        return unexpectedError(error);
    }
}

function unexpectedError(error: unknown): UnexpectedError {
    return {
        ok: false,
        errorType: "unexpected",
        error
    }
}

function jsonOptions(method: "POST" | "PUT", body?: any): RequestInit {
    return {
        method,
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    }
}

export async function put<TResponse>(path: string, body: any): Promise<ApiResponseWithData<TResponse>> {
    return fetchJson(path, jsonOptions("PUT", body));
}

export async function post(path: string, body: any): Promise<ApiResponse> {
    return fetchResponse(path, jsonOptions("POST", body));
}