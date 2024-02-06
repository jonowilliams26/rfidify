const baseUrl = 'http://localhost:5293';

type ApiResponse = Success | Error;
type ApiResponseWithData<T> = SuccessWithData<T> | Error;

type Success = {
    ok: true;
    isHttpError: false;
    isUnexpectedError: false;
    response: Response;
}

type SuccessWithData<T> = {
    ok: true;
    data: T;
    isHttpError: false;
    isUnexpectedError: false;
    response: Response;
}

export type Error = HttpError | UnexpectedError

type HttpError = {
    ok: false;
    isHttpError: true;
    isUnexpectedError: false;
    error: Response;
}

type UnexpectedError = {
    ok: false;
    isHttpError: false;
    isUnexpectedError: true;
    error: unknown;
}

export type FetchFn = (input: URL | RequestInfo, init?: RequestInit | undefined) => Promise<Response>;

async function fetchApiResponse(fetch: FetchFn, path: string, options?: RequestInit): Promise<ApiResponse> {
    try {
        const url = new URL(path, baseUrl)
        const response = await fetch(url, options);
        if (!response.ok) {
            return httpError(response);
        }
        return success(response);
    } catch (error) {
        return unexpectedError(error);
    }
}

async function fetchJson<T>(fetch: FetchFn, path: string, options?: RequestInit): Promise<ApiResponseWithData<T>> {
    try {
        const response = await fetchApiResponse(fetch, path, options);
        if (!response.ok) {
            return response;
        }
        const data = await response.response.json();
        return successWithData(data, response.response);
    } catch (error) {
        return unexpectedError(error);
    }
}

function success(response: Response): Success {
    return {
        ok: true,
        isHttpError: false,
        isUnexpectedError: false,
        response: response
    }
}

function successWithData<T>(data: T, response: Response): SuccessWithData<T> {
    return {
        ok: true,
        data,
        isHttpError: false,
        isUnexpectedError: false,
        response: response
    }
}


function httpError(response: Response): HttpError {
    return {
        ok: false,
        isHttpError: true,
        isUnexpectedError: false,
        error: response
    }
}

function unexpectedError(error: unknown): UnexpectedError {
    return {
        ok: false,
        isHttpError: false,
        isUnexpectedError: true,
        error
    } 
}

/**
 * @param path The path to the resource
 * @param body The body of the request which will be serialized to JSON
 * @returns The response deserialized from JSON to TResponse
 */
export async function putJson<TResponse>(fetch: FetchFn, path: string, body: any): Promise<ApiResponseWithData<TResponse>> {
    return fetchJson(fetch, path, jsonOptions("PUT", body));
}

/**
 * @param path The path to the resource
 * @param body The body of the request which will be serialized to JSON
 */
export async function post(fetch: FetchFn, path: string, body: any): Promise<ApiResponse> {
    return fetchApiResponse(fetch, path, jsonOptions("POST", body));
}

/**
 * @param path The path to the resource
 * @returns The response deserialized from JSON to TResponse
 */
export async function getJson<TResponse>(fetch: FetchFn, path: string): Promise<ApiResponseWithData<TResponse>> {
    return fetchJson(fetch, path);
}

export async function get(fetch: FetchFn, path: string): Promise<ApiResponse> {
    return fetchApiResponse(fetch, path);
}

function jsonOptions(method: "POST" | "PUT", body: any): RequestInit {
    return {
        method,
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    }
}