<script lang="ts">
	import { Button } from '$lib/components/ui/button';
	import { LinkBreak2 } from 'radix-icons-svelte';
</script>

<div class="min-h-full">
	<nav class="border-b">
		<div class="mx-auto max-w-7xl px-2">
			<div class="flex h-16 items-center justify-between">
				<a href="/" class="flex items-center space-x-2">
					<img class="block h-8 w-auto" src="/icon.svg" alt="logo" />
                    <span class="text-primary font-bold text-lg">RFIDify</span>
				</a>
				<Button href="/rfid/setup">
					<LinkBreak2 class="mr-2" />
					Add RFID
				</Button>
			</div>
		</div>
	</nav>

	<div class="py-10">
		<main>
			<div class="mx-auto max-w-7xl px-2">
				<slot />
			</div>
		</main>
	</div>
</div>
