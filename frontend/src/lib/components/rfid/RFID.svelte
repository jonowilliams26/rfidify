<script lang="ts">
	import type { RFID } from '$lib/api/types';
	import { Link2 } from 'radix-icons-svelte';
	import { SpotifyItem } from '$lib/components/spotify';

	export let rfid: RFID;
</script>

<SpotifyItem rfid={rfid.id} spotifyItem={rfid.spotifyItem}/>