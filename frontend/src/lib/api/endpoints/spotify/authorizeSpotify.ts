import { post, type FetchFn } from "../../fetch";

type AuthorizeSpotifyRequest = {
    code: string;
    state: string;
};

export default async function authorizeSpotify(fetch: FetchFn, request: AuthorizeSpotifyRequest) {
    return await post(fetch, '/spotify/authorize', request);
}