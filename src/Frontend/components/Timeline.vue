<template>
  <div class="flow-root">
    <ul role="list" class="mb-2">
      <li v-for="(item, index) in historyList" :key="index">
        <div class="relative pb-4">
          <span v-if="index !== historyList.length - 1" class="absolute left-3 top-5 -ml-px h-full w-0.5 bg-gray-200"
            aria-hidden="true" />
          <div class="relative flex items-start space-x-8">
            <div class="relative">
              <div class="absolute top-2.5 left-1.5  h-3 w-3 items-center justify-center rounded-lg"
                :class="[historyColor(item.historyType)]">
              </div>
            </div>
            <div class="ml-6 my-1">
              <div>

                <span class="text-sm text-gray-100">
                  <div v-if="item.historyType == HistoryType.WORKED_HOUR_REGISTRATION" class="inline-flex items-center rounded-md bg-success/50 p-1 text-xs font-medium text-white mr-1">
                    {{ sprintMessage(item).workedHour }}h
                  </div>
                  <div v-if="item.historyType == HistoryType.PLANNED_HOUR_REGISTRATION" class="inline-flex items-center rounded-md bg-warning/50 p-1 text-xs font-medium text-white mr-1">
                    {{ sprintMessage(item).plannedHour }}h
                  </div>
                  <div v-if="item.historyType == HistoryType.FINISH" class="inline-flex items-center rounded-md bg-primary/50 p-1 text-xs font-medium text-white mr-1">
                    <i class="fas fa-check"/>
                  </div>
                  <div v-if="item.historyType == HistoryType.DELAY" class="inline-flex items-center rounded-md bg-error/50 px-2 py-1 text-xs font-medium text-white mr-1">
                    <i class="fas fa-exclamation"/>
                  </div>
                  <div v-if="item.historyType == HistoryType.REMOVE" class="inline-flex items-center rounded-md bg-pink-500/50 px-2 py-1 text-xs font-medium text-white mr-1">
                    <i class="fas fa-times"/>
                  </div>
                  <div v-if="item.historyType == HistoryType.COMMENT" class="inline-flex items-center rounded-md bg-info/50 px-2 py-1 text-xs font-medium text-white mr-1">
                    <i class="fas fa-comment"/>
                  </div>
                    {{ historyAuthor(item) }}
                </span>
                <p class="text-sm text-gray-400">Sprint: {{ sprintMessage(item).sprint }}</p>
                <p class="text-xs text-gray-500">{{ formatDate(item.date) }}</p>
              </div>
              <div class="mt-2 text-xs text-white">
                <div v-if="item.historyType == HistoryType.DELAY">
                <p class="capitalize-block">Delay Reason: {{ sprintMessage(item).message }}</p>
                </div>
                <div v-else-if="item.historyType == HistoryType.WORKED_HOUR_REGISTRATION">
                <p class="capitalize-block">Comment: {{ sprintMessage(item).comment }}</p>
                </div>
                <div v-else-if="item.historyType == HistoryType.REMOVE">
                <p class="capitalize-block">{{ sprintMessage(item).message }}</p>
                </div>
                <div v-else-if="item.historyType == HistoryType.COMMENT">
                <p class="capitalize-block">{{ sprintMessage(item).message }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </li>
    </ul>
  </div>
</template>

<script>
import { HistoryType } from '@/models/HistoryTypes.ts'

export default defineComponent({
  data() { return { HistoryType: HistoryType } },
  name: 'TimeLine',
  props: {
    historyList: {
      type: Object,
      required: true,
    },
  },
  computed: {
    historyColor() {
      return (type) => {
        switch (type) {
          case HistoryType.DELAY:
            return 'bg-error'
          case HistoryType.FINISH:
            return 'bg-primary'
          case HistoryType.PLANNED_HOUR_REGISTRATION:
            return 'bg-warning'
          case HistoryType.WORKED_HOUR_REGISTRATION:
            return 'bg-success'
          case HistoryType.REMOVE:
            return 'bg-pink-500'
          case HistoryType.COMMENT:
            return 'bg-info'
          default:
            return 'bg-gray-200'
        }
      }
    },
  },
  methods: {
    sprintMessage(historyItem) {
      let sprint = '';
      let workedHour = '';
      let plannedHour = '';
      let comment = '';
      let message = "";
      if (historyItem.sprint) {
        sprint = historyItem.sprint.name
        workedHour = historyItem.workedHour
        plannedHour = historyItem.plannedHour
        comment = historyItem.comment
        message = historyItem.value;
      }
      return {sprint, workedHour, plannedHour, comment, message}
    },

    historyAuthor(historyItem) {
      const employee = historyItem.employee;
      return employee;
    },

    formatDate(value) {
      const date = new Date(value)
      const year = date.getFullYear()
      const month = date.toLocaleString('en-US', { month: 'long' })
      const day = date.getDate()
      const hour = date.getHours()
      const minutes = date.getMinutes()
      const seconds = date.getSeconds()

      return `${year} ${month} ${day} ${hour}:${minutes}:${seconds}`
    },
  },
})
</script>
