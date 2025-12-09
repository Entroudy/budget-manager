<template>
  <div>
    <div class="flex items-center md:items-start justify-between flex-col md:flex-row">
      <div class="flex items-center justify-between order-2 md:order-1">
        <div class="flex flex-col md:flex-row space-y-4 md:space-y-0">
          <div class="flex flex-col md:flex-row order-2 md:order-1">

            <!-- Bulk Actions -->
            <template v-if="$slots['bulk-actions']">
              <template v-if="selectedIds.length > 0 || dummyTable && selectedIndexes.length > 0">
                <div class="flex md:ml-2 md:mr-4 justify-center order-2 md:order-1 mb-4 md:mb-0">
                  <slot name="bulk-actions" />
                </div>
              </template>
            </template>

            <div class="flex space-x-4 justify-between order-1 md:order-2 mt-4 mb-4 md:mt-0">
              <!-- Search -->
              <div v-if="search">
                <label for="search">Keresés</label>
                <input id="search" v-model="searchValue" type="text" class="capitalize-block w-full"
                  :placeholder="`${$t('searching')}`" :title="hoverText">
              </div>
              <!-- Page Size -->
              <div v-if="!dummyTable && search">
                <label for="pageSize">Oldalméret</label>
                <select name="pageSize" v-model="pagination.pageSize" id="pageSize" class="w-18"
                  @change="changePageSize($event)">
                  <option :value="pageSize" v-for="(pageSize, index) in pageSizes" :key="`page-size-${index}`">{{
                    pageSize
                  }}
                  </option>
                </select>
              </div>
            </div>
          </div>
          <!-- Heading Actions -->
          <div v-if="$slots['heading-action']" class="flex flex-col md:flex-row md:order-2">
            <slot name="heading-action" />
          </div>
        </div>
      </div>
      <div v-if="$slots['main-action']" class="md:order-1 flex flex-col items-center md:items-end">
        <h3 v-if="title" class="mb-2 text-md md:text-lg">
          <span class="text-sm font-bold text-primary dark:text-white mb-0 capitalize-block"></span>
          <span class="text-bold text text-primary dark:text-white capitalize-block">{{ title.top
            }} <span class="font-bold">{{ title.bottom }}</span> ({{
              itemsOnCurrentPage }} of {{ pagination.totalCount }})</span>
        </h3>
        <slot name="main-action" />
      </div>
    </div>

    <div v-if="$slots['extra-filters']" class="my-4">
      <slot name="extra-filters" />
    </div>

    <h3 class="flex items-start flex-col mt-4 mb-2 md:hidden" v-if="title">
      <span class="text-sm font-bold text-primary dark:text-white mb-0 capitalize-block">{{ title.top
        }}</span>
      <span class="text-bold text text-primary dark:text-white capitalize-block">{{ title.bottom }} ({{
        itemsOnCurrentPage }} of {{ pagination.totalCount }})</span>
    </h3>
    <div class="responsive-table" :class="[quickCreate ? 'pb-32' : '', lessPadding ? 'pb-8' : '']">
      <table class="mt-3 w-full whitespace-nowrap text-left">
        <thead class="border-b border-gray-300 dark:border-white text-sm leading-6">
          <slot name="extra-headings" />
          <tr>
            <th v-if="displayCheckbox" class="w-14 text-center"><input type="checkbox" class="!m-0"
                @change="selectAllRows" :disabled="tableData.length === 0"
                :class="[tableData.length === 0 ? 'cursor-not-allowed' : '']" :checked="allSelected" /></th>
            <template v-for="(heading, i) in headings" :key="`${heading}-${i}`">
              <template v-if="heading.key !== 'actions' && heading.key !== 'details'">
                <th class="font-bold cursor-pointer uppercase py-3 px-2 text-primary"
                  :class="[heading.classes, smallTable && i > 0 ? 'text-center' : '', smallTable ? 'text-xs' : '']"
                  @click="sortTable(heading.key)">
                  <span>
                    {{ $t(heading.name) }}
                  </span>
                  <span v-if="sortCount > 0 && sortBy === heading.key" class="ml-1">
                    <i :class="[sortCount == 1 ? 'fas fa-long-arrow-alt-up' : 'fas fa-long-arrow-alt-down']"
                      class="text-gray-600" />
                  </span>
                </th>
              </template>
              <template v-else>
                <th class="text-bold uppercase" :class="heading.classes">
                  <slot name="headingItem" :data="heading.name">
                    {{ $t(heading.name) }}
                  </slot>
                </th>
              </template>
            </template>
          </tr>
        </thead>
        <tbody>


          <template v-for="(data, index) in tableData" :key="index">
            <tr class="hover:bg-indigo-50 cursor-pointer" @click="openRowItem(data, $event)">
              <td v-if="displayCheckbox"
                class="text-center text-gray-900 border-t dark:text-white border-gray-200 dark:border-white py-3 pl-2 text-sm">
                <input type="checkbox"
                  :checked="isSelected((data.id || index), index)"
                  @change="toggleRowSelection((data.id || index), $event, index)" />
              </td>
              <template v-for="(heading, i) in headings" :key="`${heading}-${i}`">
                <td v-if="heading.key !== 'actions' && heading.key !== 'details'"
                  class="text-gray-900 border-t dark:text-white border-gray-200 dark:border-white py-3 px-2 text-sm"
                  :class="[smallTable ? 'min-w-75px' : 'min-w-200px', smallTable && i > 0 ? 'text-center' : '', heading.classes]">
                  <slot :id="data.id" name="rowItem" :data="[data, index]" :heading="heading.key">
                    <template v-if="heading.editMode">
                      <template v-if="heading.dataType === 'text'">
                        <input type="text" :value="data[heading.key]" class="w-full"
                          @blur="editRowItemValue(data.id || index, heading.key, $event, data)">
                      </template>
                      <template v-if="heading.dataType === 'number'">
                        <template v-if="heading.useFloat">
                          <input type="number" :value="data[heading.key]" min="0" class="w-full"
                            @blur="editRowItemValue(data.id || index, heading.key, $event, data)">
                        </template>
                        <template v-if="!heading.useFloat">
                          <input type="number" :value="data[heading.key]" min="0" class="w-full"
                            @blur="editRowItemValue(data.id || index, heading.key, $event, data)">
                        </template>
                      </template>
                      <template v-if="heading.dataType === 'select'">
                        <select @blur="editRowItemValue(data.id || index, heading.key, $event, data)" class="w-full">
                          <option :value="option.value" :selected="data[heading.key] == option.value"
                            :name="heading.key" v-for="option in findOptionsByKey(heading.key)"
                            :key="`option-item-${index}`">{{ option.name }}</option>
                        </select>
                      </template>
                    </template>
                    <template v-if="!heading.editMode">
                      <template v-if="data[heading.key]">
                        {{ data[heading.key] }}
                      </template>
                      <template v-else>
                        {{ data[heading.key] !== undefined && data[heading.key] !== null && data[heading.key] !== '' ?
                          data[heading.key] : '' }}
                      </template>
                    </template>
                  </slot>
                </td>
              </template>
              <td v-if="$slots.actions"
                class="text-end border-t dark:border-white border-gray-200 py-3 text-sm space-x-3">
                <slot :id="data.id" name="actions" :data="data" />
              </td>
              <td v-if="$slots.details" class="text-end border-t dark:border-white border-gray-200 py-3 text-sm">
                <slot :id="data.id" name="details" :data="data" />
              </td>
            </tr>
          </template>
          <tr v-if="showNewItemRow" class="bg-gray-100 text-gray-900">
            <td class="p-2.5" v-if="displayCheckbox"></td>
            <td class="p-2.5" v-for="(heading, index) in filteredHeadings" :key="heading.name"
              :class="[heading.classes]">
              <template v-if="!heading.displayOnly">
                <span v-if="newItem.inventoryItemType && heading.key == 'inventoryItemType'">{{
                  newItem.inventoryItemType }}</span>
                <template v-if="heading.dataType === 'text'">
                  <input class="w-full" type="text" @input="addNewValue(heading.key, $event)"
                    :value="newItem[heading.key]" :disabled="heading.disableQuickEdit"
                    :class="[heading.disableQuickEdit ? '!bg-transparent !text-white !shadow-none hidden' : '']">
                </template>
                <template v-if="heading.dataType === 'number'">
                  <input class="w-full" type="number" @input="addNewValue(heading.key, $event)"
                    :value="newItem[heading.key]" min="0" :disabled="heading.disableQuickEdit"
                    :class="[heading.disableQuickEdit ? '!bg-transparent !text-white !shadow-none hidden' : '']">
                </template>
                <template v-if="heading.dataType === 'checkbox'">
                  <input type="checkbox" class="!m-0" v-model="newItem[heading.key]" />
                </template>
                <template v-if="heading.dataType === 'select'">
                  <select v-model="newItem[heading.key]" @input="addNewValue(heading.key, $event)" class="w-full">
                    <option value="" disabled />
                    <option :value="option.value" :name="heading.key" v-for="option in findOptionsByKey(heading.key)"
                      :key="`option-item-${index}`">{{ option.name }}</option>
                  </select>
                </template>
                <template v-if="heading.dataType === 'datepicker'">
                  <Datepicker class="!w-48" :name="heading.key" :id="heading.key" auto-apply :enable-time-picker="false"
                    dark v-model="newItem[heading.key]"></Datepicker>
                </template>
                <template v-if="heading.dataType === 'search-input'">
                  <div class="flex">
                    <button v-if="heading.addFromQuickCreate && newItem.bedrockObjectId"
                      class="btn btn-xs !rounded-r-none bg-green-500 h-9" @click="triggerCreate"><i
                        class="fas fa-plus"></i></button>
                    <multiselect v-floating-select v-model="newItem[heading.key]"
                      :options="findOptionsByKey(heading.key)" class="!w-60"
                      :class="[quickCreate && heading.bindTo && newItem.bedrockObjectId ? 'quick-create' : '']"
                      @select="multiOptionSelected(heading.key, heading.bindTo, $event)" label="name" track-by="value"
                      :max-height="550" :hideSelected="true" :multiple="heading.allowMultiSelect"
                      :custom-label="customLabel" :filter-method="customFilter">
                      <template #singleLabel="{ option }">
                        <span>{{ option.name }}</span>
                      </template>
                      <template #option="{ option }">
                        <span>{{ option.name }}</span>
                      </template>
                    </multiselect>
                  </div>
                </template>
              </template>

            </td>
            <td></td>
          </tr>

          <tr class="text-center" v-if="quickCreate || dummyTable">
            <td :colspan="headings.length + 2">
              <button v-if="!showNewItemRow" class="btn btn-sm bg-primary text-white mt-4"
                @click="initializeNewRowItem">
                <i class="fas fa-plus"></i>
              </button>
              <div v-if="showNewItemRow" class="space-x-3">
                <button class="btn btn-sm bg-gray-500 text-white mt-4" @click="closeNewItemRow">
                  <i class="fas fa-times"></i>
                </button>
                <button class="btn btn-sm bg-primary text-white mt-4" @click="saveNewItem"
                  :disabled="!isNewItemValid" :class="[!isNewItemValid ? 'cursor-not-allowed' : '']">
                  <i class="fas fa-check"></i>
                </button>
              </div>
            </td>
          </tr>
          <!-- No table items -->
          <tr v-if="tableData.length === 0 && !dummyTable && !quickCreate">
            <td :colspan="headings.length + 1">
              <div class="bg-gray-500 p-3.5 rounded-bl-md rounded-br-md">
                <h5 class="text-white text-center font-bold capitalize">
                  {{ $t('no_data_to_display') }}
                </h5>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
      <div class="mt-6 mr-6 mb-2">
        <CustomPagination
          v-if="pagination && tableData.length > 0 && pagination.totalPages > 0 && pagination.totalPages > 1 || pagination.currentPage > 1"
          :pagination="pagination" @on-page-change="changePage" small />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CustomTable',
  props: {
    headings: {
      type: Object,
      required: true
    },
    tableData: {
      type: Array,
      required: true,
      default: () => []
    },
    title: {
      type: Object,
      required: false
    },
    search: {
      type: Boolean,
      default: false
    },
    searchQuery: {
      type: String,
      default: ''
    },
    quickCreate: {
      type: Boolean,
      default: false
    },
    displayCheckbox: {
      type: Boolean,
      default: true
    },
    dummyTable: {
      type: Boolean,
      default: false
    },
    sortCount: {
      type: Number,
      required: false
    },
    sortBy: {
      type: String,
      required: false
    },
    pagination: {
      type: Object,
      required: false,
      default: {
        currentPage: 1,
        pageSize: 50,
        totalCount: 0,
        totalPages: 0,
      },
    },
    smallTable: {
      type: Boolean,
      required: false
    },
    lessPadding: {
      type: Boolean,
      required: false
    },
    totalPages: {
      type: Number,
      default: 0
    },
    hoverText: {
      type: String,
      default: ''
    },
    tableName: {
      type: String,
      default: ''
    },
    newItemOptions: {
      type: Array,
      default: () => []
    },
  },
  emits: ['on-table-sort', 'on-page-change', 'on-search', 'on-data-selected', 'on-new-item-added', 'on-row-item-click', 'on-page-size-change', 'on-row-item-edit', 'quick-create-clicked', 'query-items', 'on-index-selected'],
  data() {
    return {
      searchValue: '',
      selectedIds: [],
      selectedIndexes: [],
      showNewItemRow: false,
      newItem: {},
      pageSizes: ['10', '25', '50', '100']
    }
  },
  computed: {
    itemsOnCurrentPage() {
      if (!this.tableData || this.tableData.length === 0) {
        return 0
      }
      if (this.pagination?.currentPage < this.pagination?.totalPages) {
        return this.pagination.pageSize;
      } else {
        return this.pagination?.totalCount % this.pagination?.pageSize || this.pagination?.pageSize || 0;
      }
    },
    filteredHeadings() {
      if (!this.headings || !Array.isArray(this.headings)) {
        return []
      }
      return this.headings.reduce((acc, heading) => {
        if (heading?.dataType) {
          acc.push(heading);
        }
        return acc;
      }, [])
    },
    dataHeadingsLength() {
      if (!this.headings) return 0
      return this.$slots.actions ? this.headings.length - 1 : this.headings.length
    },
    allSelected: {
      get() {
        return this.tableData?.length > 0 && this.selectedIds.length === this.tableData.length;
      },
      set(value) {
        if (value) {
          if (!this.tableData || this.tableData.length === 0) {
            return;
          }
          if (this.dummyTable) {
            this.selectedIndexes = Array.from({ length: this.tableData.length }, (_, i) => i)

          }
          if (this.tableData[0]?.id && !this.dummyTable) {
            this.selectedIds = this.tableData.map((row, index) => {
              if(this.tableName == "orderTemplateRemove")
              {
                return row.id
              }
              if (!row.id) {
                return index
              }
              if (row.productId) {
                return row.productId
              }
              return row.id
            })
            return
          }
          if (this.tableData[0]?.bedrockObjectId) {
            this.selectedIds = Array.from({ length: this.tableData.length }, (_, i) => i)
          }
        }
        if (!value) {
          this.selectedIds = [];
          this.selectedIndexes = [];
        }
      }
    },
    isNewItemValid() {
      if (!this.headings || !Array.isArray(this.headings) || !this.newItem) {
        return false
      }
      const enabledKeys = this.headings
        .filter(heading => !heading?.disableQuickEdit)
        .map(heading => heading?.key)
        .filter(key => key !== undefined);

      const filteredItem = Object.keys(this.newItem)
        .filter(key => enabledKeys.includes(key))
        .reduce((obj, key) => {
          obj[key] = this.newItem[key];
          return obj;
        }, {});

      return (
        Object.keys(filteredItem).length > 0 &&
        Object.values(filteredItem).every(
          value => value !== "" && value !== null && value !== undefined
        )
      );
    }
  },
  watch: {
    searchQuery(newValue, oldValue) {
      if (this.searchValue === '') {
        this.searchValue = newValue
      }
    },
    searchValue(newValue, oldValue) {
      if (newValue.length >= 1) {
        this.$emit('on-search', newValue)
      }
      if (newValue.length === 0) {
        this.$emit('on-search', '')
      }
    },
    selectedIds: {
      handler(newValue) {
        this.$emit('on-data-selected', newValue)
      },
      deep: true
    },
    selectedIndexes: {
      handler(newValue) {
        this.$emit('on-index-selected', newValue)
      },
      deep: true
    },
    tableData: {
      handler(newValue) {
        if (newValue) {
          this.selectedIds = []
          this.selectedIndexes = []
          this.setNewItemObject()
        }
      },
      deep: true
    },
    newItemOptions: {
      handler(newValue) {
        if (newValue && this.newItem?.inventoryItemProduct) {
          this.newItem.inventoryItemProduct = ""
        }
      },
      deep: true
    },
  },
  mounted() {
    if (this.dummyTable || this.quickCreate) this.initializeNewRowItem()
  },
  beforeUnmount() {
    this.selectedIds = []
    this.selectedIndexes = []
    this.showNewItemRow = false
    this.newItem = {}
  },
  methods: {
    customFilter(option, searchTerm) {
      if (!option || !searchTerm) return false
      const name = option.name?.toLowerCase() || '';
      const value = option.value?.toLowerCase() || '';
      const search = searchTerm.toLowerCase();

      return name.includes(search) || value.includes(search);
    },
    customLabel(option) {
      if (!option) return ''
      return `${option.name || ''} (${option.value || ''})`;
    },
    triggerCreate() {
      this.$emit('quick-create-clicked', this.newItem.bedrockObjectId)
    },
    openRowItem(data, event) {
      if (!event.target.type) {
        this.$emit('on-row-item-click', data)
      }
    },
    multiOptionSelected(key, bindTo, selectedOption) {
      if (!this.newItem) return
      if (this.dummyTable || this.quickCreate) {
        if (key === 'bedrockObjectId') {
          this.$emit('query-items', this.newItem.bedrockObjectId)
          if (bindTo && this.newItem.bedrockObjectId?.bindValue) {
            this.newItem[bindTo] = this.newItem.bedrockObjectId.bindValue
          }
          return
        }
        if (!bindTo) return
        if (this.newItem[key]?.bindValue) {
          this.newItem[bindTo] = this.newItem[key].bindValue
        }
      }
    },
    findOptionsByKey(key) {
      if (!this.newItemOptions || this.newItemOptions.length === 0) {
        return []
      }
      const found = this.newItemOptions.find(item => item.key === key)
      return found?.options || []
    },
    saveNewItem() {
      this.$emit('on-new-item-added', this.newItem)
      this.newItem = {}
      this.showNewItemRow = false
      this.initializeNewRowItem()
    },
    closeNewItemRow() {
      this.newItem = {}
      this.showNewItemRow = false
    },
    initializeNewRowItem() {
      this.showNewItemRow = !this.showNewItemRow
      this.setNewItemObject()
    },
    setNewItemObject() {
      if (!this.headings || !Array.isArray(this.headings)) {
        return
      }
      this.headings.forEach(item => {
        if (!item?.displayOnly) {

          if (item.dataType === 'datepicker' || item.dataType === 'text' || item.dataType === 'select' || item.dataType === 'search-input') {
            this.newItem[item.key] = ''
          }
          if (item.dataType === 'number') {
            this.newItem[item.key] = null
            if (item.key == 's_PackageCount' || item.key == 'l_PackageCount' || item.key == 'xL_PackageCount') {
              this.newItem[item.key] = 0
            }

          }
          if (item.dataType === 'checkbox') {
            this.newItem[item.key] = false
          }
        }
      });
    },
    addNewValue(key, event) {
      if (!this.newItem || !event?.target) return
      this.newItem[key] = event.target.value
    },
    changePageSize(event) {
      this.selectedIds = []
      this.$emit('on-page-size-change', event.target.value)
    },
    editRowItemValue(id, key, event, row) {
      const value = event.target.type == 'number' ? parseInt(event.target.value) : event.target.value
      this.$emit('on-row-item-edit', {
        id: id,
        key: key,
        value: value,
        row: row
      })
    },
    selectAllRows(event) {
      this.allSelected = event.target.checked;
    },
    toggleRowSelection(id, event, index) {
      if (event.target.checked) {
        if (!this.dummyTable) this.selectedIds.push(id); // TODO: maybe change condition it if breaks other dummy table
        this.selectedIndexes.push(index)
      } else {
        if (!this.dummyTable) this.selectedIds = this.selectedIds.filter(selectedId => selectedId !== id);  // TODO: maybe change condition it if breaks other dummy table
        this.selectedIndexes = this.selectedIndexes.filter(selectedId => selectedId !== index);

      }
    },
    isSelected(id, index) {
      if (this.allSelected) {
        return true
      }
      if (this.dummyTable) {
        return this.selectedIndexes.includes(index);
      }
      return this.selectedIds.includes(id);
    },
    sortTable(value) {
      this.$emit('on-table-sort', value)
    },
    changePage(pageNumber) {
      this.selectedIds = []
      this.$emit('on-page-change', pageNumber)
    }
  }
}
</script>
