import type { PageServerLoad } from './$types';
import { getRFIDs } from '$lib/api/endpoints/getRFIDs';
import { error } from '@sveltejs/kit';

export const load = (async () => {
    const response = await getRFIDs();
    if(!response.ok) {
        error(500, 'Failed to load RFIDs');
    }
    return {
        rfids: response.data
    };
}) satisfies PageServerLoad;