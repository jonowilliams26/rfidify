import type { PageServerLoad } from './$types';

export const load = (async ({ url }) => {

    const code = url.searchParams.get('code');
    const state = url.searchParams.get('state');
    const error = url.searchParams.get('error');

    if (error) {
        return {
            error
        };
    }


    
}) satisfies PageServerLoad;