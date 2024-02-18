<script lang="ts">
	import { Skeleton } from "$lib/components/ui/skeleton";
</script>

<ul role="list" class="divide-y">
    {#each Array(5) as _}
        <li class="flex items-center justify-between py-4">
            <div class="flex items-center space-x-2">
                <Skeleton class="h-14 w-14 rounded-lg" />
                <div class="space-y-2">
                    <Skeleton class="h-4 w-40 rounded-lg" />
                    <Skeleton class="h-4 w-24 rounded-lg" />
                </div>
            </div>
            <Skeleton class="h-8 w-8 rounded-lg" />
        </li>
    {/each}
</ul>