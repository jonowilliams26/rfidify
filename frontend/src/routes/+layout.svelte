<script lang="ts">
	import '../app.pcss';
	import { HamburgerMenu } from 'radix-icons-svelte';
	import { Button } from '$lib/components/ui/button';
	import { Toaster } from "$lib/components/ui/sonner";

	const navigation = [
		{ name: 'Dashboard', href: '/' },
		{ name: 'Scan', href: '/scan' }
	];

	let isOpen = false;

	function toggleMenu() {
		isOpen = !isOpen;
	}

	function handleError(event: Event) {
		console.error("test test");
	}
</script>

<Toaster />

<div class="min-h-full">
	<nav class="border-b border-accent">
		<div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
			<div class="flex h-16 justify-between">
				<div class="flex">
					<div class="flex flex-shrink-0 items-center">
						<img class="block h-8 w-auto" src="/spotify-logo.png" alt="RFIDify" />
						<p class="pl-1 font-bold text-primary">RFIDify</p>
					</div>
					<div class="hidden sm:-my-px sm:ml-6 sm:flex sm:space-x-8">
						{#each navigation as item}
							<a
								href={item.href}
								class="inline-flex items-center px-1 pt-1 text-sm font-medium hover:border-b hover:border-primary hover:text-foreground/80"
							>
								{item.name}
							</a>
						{/each}
					</div>
				</div>

				<div class="-mr-2 flex items-center sm:hidden">
					<Button variant="outline" size="icon" on:click={toggleMenu}>
						<HamburgerMenu className="h-4 w-4" />
					</Button>
				</div>
			</div>
		</div>

		{#if isOpen}
			<div class="sm:hidden" id="mobile-menu">
				<div class="space-y-1 pb-3 pt-2">
					{#each navigation as item}
						<a
							href={item.href}
							class="block border-l-4 border-transparent py-2 pl-3 pr-4 font-medium hover:border-primary hover:text-foreground/80"
						>
							{item.name}
						</a>
					{/each}
				</div>
			</div>
		{/if}
	</nav>

	<div class="py-10">
		<main>
			<div class="mx-auto max-w-7xl sm:px-6 lg:px-8">
				<slot />
			</div>
		</main>
	</div>
</div>
