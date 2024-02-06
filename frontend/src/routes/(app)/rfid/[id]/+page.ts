import type { PageLoad } from './$types';

export const load = (async ({ params, fetch }) => {
    
    const { id } = params;

    if (id === 'new') {
        return {
            test: "balh"
        }
    }
    
    return {
        hello: "world"
    };
}) satisfies PageLoad;