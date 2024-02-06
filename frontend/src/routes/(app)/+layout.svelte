<script lang="ts">
	import { HamburgerMenu, Rocket } from 'radix-icons-svelte';
	import { Button } from '$lib/components/ui/button';

	const navigation = [
		{ name: 'Dashboard', href: '/' },
		{ name: 'Scan', href: '/scan' }
	];

	let isOpen = false;

	function toggleMenu() {
		isOpen = !isOpen;
	}
</script>

<div class="min-h-full">
	<nav class="border-b border-accent">
		<div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
			<div class="flex h-16 items-center justify-between">
				<div class="flex">
					<a href="/">
						<img class="block h-8 w-auto" src="/spotify-logo.png" alt="RFIDify" />
					</a>

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
				<Button href="/rfid/scan" size="sm">
					<Rocket class="mr-1.5" />
					New RFID
				</Button>
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
			<div class="mx-auto max-w-7xl px-3 lg:px-8">
				<slot />
			</div>
		</main>
	</div>
</div>
