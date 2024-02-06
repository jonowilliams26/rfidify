<script lang="ts">
	import { Rocket } from 'radix-icons-svelte';
	import { Button } from '$lib/components/ui/button';

	const navigation = [
		{ name: 'Dashboard', href: '/' }
	];
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
			</div>
		</div>
	</nav>

	<div class="py-10">
		<main>
			<div class="mx-auto max-w-7xl px-3 lg:px-8">
				<slot />
			</div>
		</main>
	</div>
</div>
