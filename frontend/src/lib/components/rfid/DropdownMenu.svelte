<script lang="ts">
    import * as DropdownMenu from "$lib/components/ui/dropdown-menu";
	import { DotsHorizontal, Pencil1, Trash } from "radix-icons-svelte";
	import { Button } from "../ui/button";

    export let rfid: string;
</script>

<DropdownMenu.Root>
    <DropdownMenu.Trigger asChild let:builder>
        <Button builders={[builder]} variant="ghost" size="icon" class="h-6 w-6">
            <DotsHorizontal />
        </Button>
    </DropdownMenu.Trigger>
    <DropdownMenu.Content>
        <a href={`/rfid/${rfid}`}>
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
