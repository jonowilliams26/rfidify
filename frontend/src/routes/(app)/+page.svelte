<script lang="ts">
	import type { PageData } from './$types';
	import * as Card from '$lib/components/ui/card';
	import { Person, Link2, DotsHorizontal, Trash, Pencil1 } from 'radix-icons-svelte';
	import { Button } from '$lib/components/ui/button';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu';

	export let data: PageData;
</script>

<div class="grid grid-cols-1 gap-1 md:grid-cols-2 xl:grid-cols-3">
	{#each data.rfids as rfid}
		<Card.Root>
			<Card.Content class="flex space-x-3 p-4 text-xs">
				<img src={rfid.image} alt={rfid.name} class="h-16 w-16" />
				<div class="flex w-full flex-col truncate">
					<div class="flex w-full items-start justify-between space-x-4 truncate pb-0.5">
						<h2 class="truncate text-sm font-semibold">{rfid.name}</h2>
						<DropdownMenu.Root>
							<DropdownMenu.Trigger asChild let:builder>
								<Button builders={[builder]} variant="ghost" size="icon" class="h-6 w-6">
									<DotsHorizontal />
								</Button>
							</DropdownMenu.Trigger>
							<DropdownMenu.Content>
								<a href={`/rfid/${rfid.id}`}>
									<DropdownMenu.Item>
										<Pencil1 class="mr-1.5" />
										Edit
									</DropdownMenu.Item>
								</a>

								<DropdownMenu.Separator />
								<DropdownMenu.Item>
									<Trash class="mr-1.5" />
									Delete
								</DropdownMenu.Item>
							</DropdownMenu.Content>
						</DropdownMenu.Root>
					</div>
					<div class="space-y-1 truncate text-xs text-muted-foreground">
						{#if rfid.type === 'Track' || rfid.type === 'Album'}
							<div class="flex space-x-1">
								<Person />
								<span>{rfid.artists}</span>
							</div>
						{/if}
						{#if rfid.type === 'Playlist'}
							<span class="truncate">{rfid.description}</span>
						{/if}
						<div class="flex space-x-1">
							<Link2 />
							<span>{rfid.id}</span>
						</div>
					</div>
				</div>
			</Card.Content>
		</Card.Root>
	{/each}
</div>
