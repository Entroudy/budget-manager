<template>
  <div :class="classes" class="flex justify-end items-center">
    <button :disabled="isPreviousDisabled" class="btn bg-brand-darkGray"
      :class="[isPreviousDisabled ? 'cursor-not-allowed' : '', small ? 'btn-xs mr-2' : 'btn-sm']" @click="changePage(-1)">
      <i class="fas fa-arrow-left text-white" :class="[small ? 'text-base' : 'text-xl']" />
    </button>
    <div class="mx-2">
      <b class="text-brand-darkGray">{{ pagination.currentPage }} of {{ pagination.totalPages }}</b>
    </div>
    <button :disabled="isNextDisabled" class="btn bg-brand-darkGray"
      :class="[isNextDisabled ? 'cursor-not-allowed' : '', small ? 'btn-xs ml-2' : 'btn-sm']" @click="changePage(1)">
      <i class="fas fa-arrow-right text-white" :class="[small ? 'text-base' : 'text-xl']" />
    </button>
  </div>
</template>

<script>
export default {
  name: 'CustomPagination',
  props: {
    pagination: { type: Object, required: true },
    classes: { type: String, default: '' },
    small: { type: Boolean, default: false }
  },
  emits: ['on-page-change'],
  computed: {
    isPreviousDisabled() {
      return this.pagination.currentPage === 1
    },
    isNextDisabled() {
      return this.pagination.currentPage === this.pagination.totalPages || this.pagination.totalPages === 0
    }
  },
  methods: {
    changePage(pageNumber) {
      this.$emit('on-page-change', pageNumber)
    }
  }
}
</script>
