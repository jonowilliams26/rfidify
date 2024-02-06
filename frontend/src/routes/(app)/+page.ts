import type { PageLoad } from './$types';
import getRFIDs from '$lib/api/endpoints/getRFIDs';
import { error } from '@sveltejs/kit';

export const load = (async ({ fetch }) => {
    const response = await getRFIDs(fetch);
    if(!response.ok) {
        error(500, 'Failed to load RFIDs');
    }
    return {
        rfids: response.data
    };
}) satisfies PageLoad;