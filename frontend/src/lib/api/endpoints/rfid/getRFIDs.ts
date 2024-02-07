import { type FetchFn, getJson } from "../../fetch";
import type { RFID } from "../../types/rfid";

export default async function getRFIDs(fetch: FetchFn) {
    return await getJson<RFID[]>(fetch, '/rfids');
}