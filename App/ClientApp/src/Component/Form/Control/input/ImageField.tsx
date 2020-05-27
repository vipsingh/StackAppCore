import React from "react";
import { Upload, message } from "antd";
import { LoadingOutlined, PlusOutlined } from '@ant-design/icons';

export class ImageField extends React.Component<WidgetInfoProps, { loading: boolean, imageUrl: string }> {
    constructor(props: WidgetInfoProps) {
        super(props);

        this.state = {
            loading: false,
            imageUrl: (props.Value && props.Value.Url ? props.Value.Url: "")
        };
    }

    handleChange = (info: any) => {
        const {onChange} = this.props;

        if (info.file.status === 'uploading') {
            this.setState({ loading: true });
            return;
        }
        if (info.file.status === 'done') {
            var { response } = info.file
            if (response.Status === "success") {                
                getBase64(info.file.originFileObj, (imageUrl: string) =>
                    this.setState({
                        imageUrl,
                        loading: false,
                    }),
                );

                onChange({ FileName: response.FileName, "IsTemp": true });

            } else {
                this.setState({
                    loading: false,
                });
                message.error(response.message);
            }
        }
    }

    renderImageView(){
        const { FormatedValue } = this.props;

        return (<img alt="example" src={FormatedValue} style={{ width: "200px" }} />);
    }

    render() {
        const { UploadLink, IsViewMode} = this.props as any;
        if (IsViewMode) return this.renderImageView();

        let action =  `/${UploadLink.Url}&_ajax=1`;
        if (action.indexOf("//") >= 0) {
            action = action.replace("//", "/");
        }
        const uploadButton = (
            <div>
                {this.state.loading ? <LoadingOutlined /> : <PlusOutlined />}
                <div className="ant-upload-text">Upload</div>
            </div>
        );
        const { imageUrl } = this.state;
        return (
            <Upload
                name="file"
                listType="picture-card"
                className="avatar-uploader"
                showUploadList={false}
                action={ action }
                beforeUpload={beforeUpload}
                onChange={this.handleChange}
            >
                {imageUrl ? <img src={imageUrl} alt="avatar" style={{ width: '100%' }} /> : uploadButton}
            </Upload>
        );
    }
}

function getBase64(img: any, callback: Function) {
    const reader = new FileReader();
    reader.addEventListener('load', () => callback(reader.result));
    reader.readAsDataURL(img);
}

function beforeUpload(file: any) {
    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
    if (!isJpgOrPng) {
        message.error('You can only upload JPG/PNG file!');
    }
    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
        message.error('Image must smaller than 2MB!');
    }
    return isJpgOrPng && isLt2M;
} 