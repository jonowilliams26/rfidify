<script lang="ts">
    import type { PageData } from './$types';
    import { RFID, EmptyRFID } from '$lib/components/rfid';
    export let data: PageData;
    const { id, rfid } = data;
</script>

{#if !rfid}
    <EmptyRFID id={id} />
{:else}
    <RFID rfid={rfid} showOptionsMenu={false} />
{/if}