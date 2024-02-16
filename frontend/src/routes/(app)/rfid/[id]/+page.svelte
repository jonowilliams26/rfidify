<script lang="ts">
	import type { PageData } from './$types';
	import { page } from '$app/stores';
	import { RFID, EmptyStateRFID } from '$lib/components/rfid';
	import * as Tabs from '$lib/components/ui/tabs';
	import { Input } from '$lib/components/ui/input';
	import { goto } from '$app/navigation';
	import { Skeleton } from '$lib/components/ui/skeleton';

	const searchParams = {
		search: 'search',
		type: 'type'
	} as const;

	const types = {
		tracks: 'tracks',
		albums: 'albums',
		artists: 'artists',
		playlists: 'playlists'
	} as const;
	type Type = keyof typeof types;

	export let data: PageData;
	let debounceTimer: number;
	$: search = $page.url.searchParams.get(searchParams.search) || '';
	$: type = $page.url.searchParams.get(searchParams.type) || types.tracks;

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

	async function onTabClick(type: Type) {
		const query = new URLSearchParams($page.url.search);
		query.set('type', type);
		goto(`?${query}`);
	}
</script>

{#if !data.rfid.spotifyItem}
	<EmptyStateRFID rfid={data.rfid.id} />
{:else}
	<RFID rfid={data.rfid} />
{/if}

<div class="flex flex-col items-center space-x-2 space-y-2 pt-12 md:flex-row md:space-y-0">
	<Tabs.Root value={type} class="w-full md:w-[500px]">
		<Tabs.List class="grid grid-cols-4">
			<Tabs.Trigger value="tracks" on:click={() => onTabClick('tracks')}>Tracks</Tabs.Trigger>
			<Tabs.Trigger value="albums" on:click={() => onTabClick('albums')}>Albums</Tabs.Trigger>
			<Tabs.Trigger value="artists" on:click={() => onTabClick('artists')}>Artists</Tabs.Trigger>
			<Tabs.Trigger value="playlists" on:click={() => onTabClick('playlists')}>
				Playlists
			</Tabs.Trigger>
		</Tabs.List>
	</Tabs.Root>
	<Input
		type="text"
		on:input={(e) => debounce(e.currentTarget.value)}
		value={search}
		placeholder="Search tracks, albums, artists and playlists"
	/>
</div>

<ul role="list" class="divide-y pt-6">
	{#each Array(5) as _}
		<div class="flex items-center justify-between py-4">
			<div class="flex items-center space-x-2">
				<Skeleton class="h-14 w-14 rounded-lg" />
				<div class="space-y-2">
					<Skeleton class="h-4 w-40 rounded-lg" />
					<Skeleton class="h-4 w-24 rounded-lg" />
				</div>
			</div>
			<Skeleton class="h-8 w-8 rounded-full" />
		</div>
	{/each}
</ul>
