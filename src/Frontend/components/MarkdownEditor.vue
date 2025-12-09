<template>
  <template v-if="previewOnly">
    <MdPreview :theme="state.theme" @onChange="onChange()" previewTheme="github" :editorId="'preview-only'"
      :modelValue="modelValue" />
  </template>
  <template v-if="!previewOnly">
    <MdEditor :modelValue="modelValue" @onChange="onChange($event)" language="en-US" previewTheme="github"
      :toolbars="toolbars" />
  </template>
</template>

<script>
import { defineComponent, reactive } from 'vue'
import { MdEditor, MdPreview } from 'md-editor-v3'
import 'md-editor-v3/lib/style.css'


export default defineComponent({
  name: 'MarkdownEditor',
  components: {
    MdEditor,
    MdPreview
  },
  props: {
    previewOnly: {
      type: Boolean,
      required: false,
      default: false,
    },
    modelValue: {
      type: String,
      required: true,
      default: '',
    },
  },
  emits: ['update:modelValue'],
  setup(props, context) {
    const state = reactive({
      theme: 'dark',
    })
    const toolbars = [
      'bold',
      'underline',
      'italic',
      'title',
      'strikeThrough',
      'quote',
      'unorderedList',
      'orderedList',
      'task',
      'codeRow',
      'code',
      'link',
      'table',
      'pageFullscreen',
      'preview',
    ]

    const onChange = (event) => {
      context.emit('update:modelValue', event)
    }

    return {
      state,
      toolbars,
      onChange
    }
  },
})
</script>

<style>
.md-editor {
  background-color: #f9fafb;
  border-color: #e5e7eb;
  border-radius: 0.475rem;
}

.md-editor-toolbar-wrapper .md-editor-toolbar-item:hover {
  background-color: #442D1C;
}

.md-editor-preview.github-theme,
.cm-content.cm-lineWrapping * {
  color: #374151;
}

.md-editor-toolbar-wrapper .md-editor-toolbar-item {
  color: #6b7280;
}

.cm-editor {
  background: #ffffff;
  }
.cm-scroller {
  background: #ffffff;
}

.md-editor-previewOnly {
  border-radius: 8px;
}

.ͼo {
  background-color: #f9fafb;
}

.md-editor-footer-item label {
  margin-bottom: 0;
  color: #442D1C;
}
.github-theme pre code .code-block {
  color: #6b7280;
}
.github-theme code {
  color: #442D1C;
}
</style>
