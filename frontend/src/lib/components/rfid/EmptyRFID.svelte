<script lang="ts">
	import * as Card from '$lib/components/ui/card';
	import { Skeleton } from '$lib/components/ui/skeleton';
	import { Link2 } from 'radix-icons-svelte';
    export let id: string;
</script>

<Card.Root>
    <Card.Content class="flex space-x-3 p-4 text-xs">
        <Skeleton class="h-16 w-16"/>
        <div class="flex flex-col text-xs text-muted-foreground space-y-2">
            <Skeleton class="h-4 w-96 mb-0.5"/>
            <Skeleton class="h-4 w-72"/>
            <div class="flex space-x-1">
                <Link2 />
                <span>{id}</span>
            </div>
        </div>
    </Card.Content>
</Card.Root>