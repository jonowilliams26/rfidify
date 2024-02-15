import { isSpotifyCredentialsSet } from '$lib/api/endpoints';
import { redirect, type Handle, error } from '@sveltejs/kit';
export const handle: Handle = async ({ event, resolve }) => {

    // No need to run check to see if credentials have been set because they are being setup
    if (event.url.pathname.startsWith("/api") || event.url.pathname.startsWith('/spotify/setup')) {
        console.log("Skipping hooks.server.ts Handle.")
        return await resolve(event);
    }

    const response = await isSpotifyCredentialsSet(event.fetch);

    if (response === true) {
        console.log("Credentials are set.");
        return await resolve(event);
    }

    if (response === false) {
        console.log("Credentials are not set. Redirecting to /spotify/setup.");
        redirect(307, '/spotify/setup');
    }

    console.log("Unexpected error trying to check if Spotify credentials are set.");
    error(500, "Unexpected error trying to check if Spotify credentials are set.");  
};