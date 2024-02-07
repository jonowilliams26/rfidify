<script lang="ts">
	import type { PageData } from './$types';
	import { page } from '$app/stores';
	import { goto } from '$app/navigation';
	import { RFID, EmptyRFID } from '$lib/components/rfid';
	import * as Tabs from '$lib/components/ui/tabs';
	import { Input } from '$lib/components/ui/input';

	export let data: PageData;
	$: searchType = $page.url.searchParams.get('type') ?? 'tracks';

	function onArtistsTabClick() {
		changeTab('artists');
	}

	function onAlbumsTabClick() {
		changeTab('albums');
	}

	function onTracksClick() {
		changeTab('tracks');
	}

	function onPlaylistsClick() {
		changeTab('playlists');
	}

    function changeTab(type: "tracks" | "albums" | "artists" | "playlists") {
        const query = new URLSearchParams($page.url.searchParams);
        query.set('type', type);
        data.loadingItems = true;
        goto(`?${query.toString()}`);
    }
</script>

{#if !data.rfid.spotifyItem}
	<EmptyRFID id={data.rfid.id} />
{:else}
	<RFID rfid={data.rfid} showOptionsMenu={false} />
{/if}

<Tabs.Root value={searchType} class="pt-12">
	<Input id="username" value="" placeholder="Search" class="mb-2" />
	<Tabs.List class="grid grid-cols-4">
		<Tabs.Trigger value="tracks" on:click={onTracksClick}>Tracks</Tabs.Trigger>
		<Tabs.Trigger value="albums" on:click={onAlbumsTabClick}>Albums</Tabs.Trigger>
		<Tabs.Trigger value="artists" on:click={onArtistsTabClick}>Artists</Tabs.Trigger>
		<Tabs.Trigger value="playlists" on:click={onPlaylistsClick}>Playlists</Tabs.Trigger>
	</Tabs.List>
</Tabs.Root>

{#if data.loadingItems}
    <p>Loading...</p>
{:else}
    <p>{JSON.stringify(data.items)}</p>
{/if}
