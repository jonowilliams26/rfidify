<script lang="ts">
	import type { PageData } from './$types';
	import { page } from '$app/stores';
	import * as Tabs from '$lib/components/ui/tabs';
	import { Input } from '$lib/components/ui/input';
	import { goto, invalidate } from '$app/navigation';
	import {
		SpotifyItemTypes,
		type SpotifyItemType,
		type SpotifyItem,
	} from '$lib/api/types';
	import { createOrUpdateRFID, getSpotifyItems } from '$lib/api/endpoints';
	import { toast } from 'svelte-sonner';
	import { SpotifyItemsList, LoadingShellSpotifyItemsList, ErrorAlert } from './components';

	const searchParams = {
		search: 'search',
		type: 'type'
	} as const;

	export let data: PageData;
	let debounceTimer: number;
	$: search = $page.url.searchParams.get(searchParams.search) ?? '';
	$: type = $page.url.searchParams.get(searchParams.type) ?? SpotifyItemTypes.track;
	$: searchPlaceholder = `Search ${type}s`;

	const debounce = (value: string) => {
		clearTimeout(debounceTimer);
		debounceTimer = setTimeout(() => {
			const query = new URLSearchParams($page.url.search);
			if (value) {
				query.set(searchParams.search, value);
			} else {
				query.delete(searchParams.search);
			}
			goto(`?${query}`, { keepFocus: true });
		}, 750);
	};

	async function onTabClick(type: SpotifyItemType) {
		const query = new URLSearchParams($page.url.search);
		query.set('type', type);
		goto(`?${query}`);
	}

	async function onLinkClick(spotifyItem: SpotifyItem) {
		const response = await createOrUpdateRFID(fetch, {
			rfid: data.rfid.id,
			spotifyUri: spotifyItem.uri
		});

		if (!response.ok) {
			toast.error('Sorry, something went wrong. Please try again.');
			return;
		}

		invalidate(`/api/rfids/${data.rfid.id}`);
	}

	async function onLoadMore(offset?: number) {
		console.log('Loading more Spotify items...');
		const response = await getSpotifyItems(fetch, type as SpotifyItemType, search, offset);
		if (response.ok) {
			return response.data;
		}

		toast.error('Sorry, something went wrong. Please try again.');
		return undefined;
	}
</script>

<div class="flex flex-col items-center space-x-2 space-y-2 pb-6 md:flex-row md:space-y-0">
	<Tabs.Root value={type} class="w-full md:w-[500px]">
		<Tabs.List class="grid grid-cols-4">
			<Tabs.Trigger
				value={SpotifyItemTypes.track}
				on:click={() => onTabClick(SpotifyItemTypes.track)}
			>
				Tracks
			</Tabs.Trigger>
			<Tabs.Trigger
				value={SpotifyItemTypes.album}
				on:click={() => onTabClick(SpotifyItemTypes.album)}
			>
				Albums
			</Tabs.Trigger>
			<Tabs.Trigger
				value={SpotifyItemTypes.artist}
				on:click={() => onTabClick(SpotifyItemTypes.artist)}
			>
				Artists
			</Tabs.Trigger>
			<Tabs.Trigger
				value={SpotifyItemTypes.playlist}
				on:click={() => onTabClick(SpotifyItemTypes.playlist)}
			>
				Playlists
			</Tabs.Trigger>
		</Tabs.List>
	</Tabs.Root>
	<Input
		type="text"
		on:input={(e) => debounce(e.currentTarget.value)}
		value={search}
		placeholder={searchPlaceholder}
	/>
</div>

{#await data.spotifyItems}
	<LoadingShellSpotifyItemsList />
{:then spotifyItems}
	{#if spotifyItems}
		<SpotifyItemsList
			data={spotifyItems}
			{onLinkClick}
			{onLoadMore}
		/>
	{:else}
		<ErrorAlert />
	{/if}
{/await}