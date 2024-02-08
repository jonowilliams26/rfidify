<script lang="ts">
	import { onMount } from 'svelte';
    import type { PageData } from './$types';
    import { PUBLIC_BASE_URL } from '$env/static/public';
    
    export let data: PageData;

    let rfids = [];

    onMount(async () => {
        console.log('on Mount', PUBLIC_BASE_URL);
        const response = await fetch(`${PUBLIC_BASE_URL}/rfids`);
        rfids = await response.json();
    });
</script>

<h1>Hello from test</h1>
<p>{PUBLIC_BASE_URL}</p>
<p>{rfids}</p>