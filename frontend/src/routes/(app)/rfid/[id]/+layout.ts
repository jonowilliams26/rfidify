import getRFIDById from '$lib/api/endpoints/rfid/getRFIDById';
import { error } from '@sveltejs/kit';
import type { LayoutLoad } from './$types';

export const load = (async ({ fetch, params }) => {

    const { id } = params;
    const response = await getRFIDById(fetch, id);

    if (response.isHttpError && response.error.status === 404) {
        return {
            rfid: {
                id: id,
                spotifyItem: undefined
            }
        };
    }

    if (!response.ok) {
        error(500, 'Failed to load RFID');
    }

    return {
        rfid: response.data
    };
}) satisfies LayoutLoad;