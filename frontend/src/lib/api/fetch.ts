type ApiResponse = Success | ApiError;
type ApiResponseWithData<T> = SuccessWithData<T> | ApiError;

type Success = {
    ok: true;
    httpResponse: Response;
};

type SuccessWithData<T> = {
    ok: true;
    data: T;
    httpResponse: Response;
};

type ApiError = HttpError | UnexpectedError;

type HttpError = {
    ok: false;
    isHttpError: true;
    error: Response;
}

type UnexpectedError = {
    ok: false;
    isHttpError: false;
    error: unknown;
};


export type FetchFn = typeof fetch;

async function executeFetch(fetch: FetchFn, path: string | URL, init?: RequestInit): Promise<ApiResponse> {
    try {
        const url = `/api${path}`
        const response = await fetch(url, init);
        if (response.ok) {
            return {
                ok: true,
                httpResponse: response,
            }
        }

        return {
            ok: false,
            isHttpError: true,
            error: response,
        }
    } catch (error) {
        console.error(error);
        return unexpectedError(error);
    }
}

async function executeFetchJson<TResponse>(fetch: FetchFn, path: string | URL, init?: RequestInit): Promise<ApiResponseWithData<TResponse>> {
    const response = await executeFetch(fetch, path, init);
    if (!response.ok) {
        return response;
    }

    try {
        const data = await response.httpResponse.json();
        return {
            ok: true,
            data,
            httpResponse: response.httpResponse,
        }
    } catch (error) {
        console.error(error);
        return unexpectedError(error);
    }
}

function unexpectedError(error: unknown): UnexpectedError {
    return {
        ok: false,
        isHttpError: false,
        error,
    }
}

function jsonRequestInit(method: "GET" | "POST" | "PUT" | "DELETE", body: any): RequestInit {
    return {
        method,
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(body),
    };
}

export async function putJson<TResponse>(path: string | URL, body: any, fetch: FetchFn): Promise<ApiResponseWithData<TResponse>> {
    const init = jsonRequestInit("PUT", body);
    return await executeFetchJson<TResponse>(fetch, path, init);
}

export async function post(path: string | URL, body: any, fetch: FetchFn): Promise<ApiResponse> {
    const init = jsonRequestInit("POST", body);
    return await executeFetch(fetch, path, init);
}

export async function get(path: string | URL, fetch: FetchFn): Promise<ApiResponse> {
    return await executeFetch(fetch, path);
}

export async function getJson<TResponse>(path: string | URL, fetch: FetchFn): Promise<ApiResponseWithData<TResponse>> {
    return await executeFetchJson<TResponse>(fetch, path);
}

export async function $delete(path: string | URL, fetch: FetchFn): Promise<ApiResponse> {
    return await executeFetch(fetch, path, { method: "DELETE" });
}