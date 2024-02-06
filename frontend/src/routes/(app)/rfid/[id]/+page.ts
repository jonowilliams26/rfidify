import type { PageLoad } from './$types';
import getRFIDById from '$lib/api/endpoints/rfid/getRFIDById';
import { error } from '@sveltejs/kit';

export const load = (async ({ params, fetch }) => {
    
    const { id } = params;
    const response = await getRFIDById(fetch, id);

    if (response.isHttpError && response.error.status === 404) {
        return {
            id: id,
            rfid: undefined
        };
    }

    if(!response.ok) {
        error(500, 'Failed to load RFID');
    }

    return {
        id: id,
        rfid: response.data
    };

}) satisfies PageLoad;