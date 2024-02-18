<script lang="ts">
	import type { PagedResponse, SpotifyItem } from '$lib/api/types';
	import { Button } from '$lib/components/ui/button';
	import { Link1 } from 'radix-icons-svelte';
	import { SpotifyItem as SpotifyItemComponent } from '$lib/components/spotify';
	import { onDestroy, onMount } from 'svelte';
	import LoadingShellSpotifyItemsList from './LoadingShellSpotifyItemsList.svelte';

	/**
	 * The paged data to display
	 */
	export let data: PagedResponse<SpotifyItem>;

	/**
	 * Function to call when the link button is clicked
	 */
	export let onLinkClick: (spotifyItem: SpotifyItem) => void;

	/**
	 * Function to call when more items should be loaded
	 */
	export let onLoadMore: (offset?: number) => Promise<PagedResponse<SpotifyItem> | undefined>;

	let loading: boolean;
	let listElement: HTMLUListElement;
	$: items = data.items;

	onMount(() => {
		listElement.addEventListener('scroll', onScroll);
	});

	onDestroy(() => {
		listElement.removeEventListener('scroll', onScroll);
	});

	async function onScroll() {
		const scrollPosition = listElement.scrollTop + listElement.clientHeight;
		const loadMoreThreshold = listElement.scrollHeight * 0.9;

		if (scrollPosition < loadMoreThreshold || !data.next || loading) {
			return;
		}

		loading = true;
		const moreData = await onLoadMore(data.offset + data.limit);
		if (moreData) {
			data = {
				...moreData,
				items: [...data.items, ...moreData.items]
			};
		}
		loading = false;
	}
</script>

<ul role="list" class="max-h-[700px] divide-y overflow-y-auto" bind:this={listElement}>
	{#each items as spotifyItem}
		<li class="flex items-center justify-between py-2">
			<SpotifyItemComponent {spotifyItem} size="sm" />
			<Button variant="ghost" size="icon" on:click={() => onLinkClick(spotifyItem)}>
				<Link1 />
			</Button>
		</li>
	{/each}
	{#if loading}
		<LoadingShellSpotifyItemsList />
	{/if}
</ul>

<style>
	/* width */
	::-webkit-scrollbar {
		width: 0px;
	}
</style>
