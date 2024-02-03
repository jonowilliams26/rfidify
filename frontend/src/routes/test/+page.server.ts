import type { PageServerLoad } from './$types';

type RFID = {
    rfid: string;
    name: string;
    type: "Track" | "Ablum" | "Artist" | "Playlist";
}

export const load = (async () => {

    const response = await fetch("http://localhost:5293/rfids");
    const rfids = await response.json();

    return {
        rfids: rfids as RFID[]
    };
}) satisfies PageServerLoad;