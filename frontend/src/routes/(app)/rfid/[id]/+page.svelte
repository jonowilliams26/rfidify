<script lang="ts">
	import type { PageData } from './$types';
	import { page } from '$app/stores';
	import * as Tabs from '$lib/components/ui/tabs';
	import { Input } from '$lib/components/ui/input';
	import { goto } from '$app/navigation';
	import { Skeleton } from '$lib/components/ui/skeleton';
	import { SpotifyItemTypes, type SpotifyItemType } from '$lib/api/types';
	import { SpotifyItem } from '$lib/components/spotify';

	const searchParams = {
		search: 'search',
		type: 'type'
	} as const;

	export let data: PageData;
	let debounceTimer: number;
	$: search = $page.url.searchParams.get(searchParams.search) ?? '';
	$: type = $page.url.searchParams.get(searchParams.type) ?? SpotifyItemTypes.track;

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
</script>

<div class="flex flex-col items-center space-x-2 space-y-2 pt-12 md:flex-row md:space-y-0">
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
		placeholder="Search tracks, albums, artists and playlists"
	/>
</div>

{#await data.spotifyItems}
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
{:then spotifyItems}
	<ul role="list" class="divide-y pt-6">
		{#each spotifyItems.items as spotifyItem}
		<div class="py-2">
			<SpotifyItem spotifyItem={spotifyItem} size="sm"/>
		</div>
		{/each}
	</ul>
{:catch error}
	<p>error loading spotify items: {error.message}</p>
{/await}
