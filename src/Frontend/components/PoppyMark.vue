<template>
  <svg
    viewBox="0 0 48 48"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    class="shrink-0"
  >
    <g transform="translate(0 10)">
      <g :transform="expand ? 'translate(-15.6 -9.4) scale(1.65)' : undefined">
      <path d="M23.8 22.4c-4.3 4.1-10.9 3.1-12.7-2.1-1.3-3.8 1.2-7.1 5.1-8.5-.6-4.7 2-8.4 6.5-8.2 2.5.1 4.4 1.6 5.4 4 4.5-2 9.3-.4 10.3 3.7.9 3.7-1.9 6.8-5.9 7.8 0 5.1-4.4 7.7-8.7 3.3Z" fill="#C94F3D" stroke="#963D31" stroke-width="1.5" stroke-linejoin="round" />
      <path d="M22.7 15.8c.7-2.8 1.1-5.5.7-8.2m2 8.8c2-2.2 4.1-4.4 6.9-5.8m-8.8 5.9c-2.1-1.8-4.3-3.4-6.8-4.4m9 4.7c2.7.4 5.5 1.2 7.6 2.8m-8.6-2.7c-1.1 2.2-1.7 4.3-1.8 6.5" stroke="#30271F" stroke-width="1.25" stroke-linecap="round" />
      <circle cx="24" cy="16.2" r="2.2" fill="#30271F" />
      </g>
    </g>
  </svg>
</template>

<script setup lang="ts">
defineProps<{ expand?: boolean }>()
</script>
