var M_formData = {
    formData: [],
    pageIndex: 1,
    pageSize:19,
    searchName: ''
}
var m_loading = {
    loading_seen: false
}

var addModels = {
    self:{},
    id: '',
    typeName: '',
    name: '',
    company: '',
    description: '',
    thumbUrl: '',
    picUrls: [],
    modelUrl: '',
    tagArrSub: [],//待提交数据
    tagArr: [],//渲染
    tag: '',
}