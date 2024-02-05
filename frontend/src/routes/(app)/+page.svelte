<script lang="ts">
	import type { PageData } from './$types';
	import * as Card from '$lib/components/ui/card';
	import { Person, Link2 } from 'radix-icons-svelte';

	export let data: PageData;
</script>

<div class="grid grid-cols-1 gap-1 md:grid-cols-2 xl:grid-cols-3">
	{#each data.rfids as rfid}
		<Card.Root>
			<Card.Content class="flex space-x-2 p-4">
				<img src={rfid.image} alt={rfid.name} class="h-16 w-16" />
				<div class="truncate">
					<h2 class="truncate pb-1 text-xs font-semibold">{rfid.name}</h2>
					<div class="flex flex-col space-y-1 text-xs text-muted-foreground">
						{#if rfid.type === 'Track' || rfid.type === 'Album'}
							<div class="flex items-center space-x-1">
								<Person />
								<span>{rfid.artists}</span>
							</div>
						{/if}
						{#if rfid.type === 'Playlist'}
							<span class="truncate">{rfid.description}</span>
						{/if}
						<div class="flex items-center space-x-1">
							<Link2 />
							<span>{rfid.rfid}</span>
						</div>
					</div>
				</div>
			</Card.Content>
		</Card.Root>
	{/each}
</div>
