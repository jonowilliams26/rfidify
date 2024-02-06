import { type FetchFn, getJson } from "../fetch";
import type { RFIDTrack, RFIDAlbum, RFIDPlaylist, RFIDArtist } from "../types/RFID";

type GetRFIDByIdResponse = RFIDTrack | RFIDAlbum | RFIDPlaylist | RFIDArtist;

export default async function getRFIDById(fetch: FetchFn, id: string) {
    return await getJson<GetRFIDByIdResponse>(fetch, `/rfids/${id}`);
}