import React from "react";
import { useForm } from "react-hook-form";
import { useState, useEffect } from "react";
import MaterialTable from '@material-table/core';
import tableIcons from "./MaterialTableIcons";
import API from '../service/api';

const MyFileUpload = () => {

    const [file, setFile] = useState([]);
    const [list, setList] = useState([]);
    const [sizeUploaded, setSizeUploaded] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const { register, handleSubmit } = useForm();

    const getList = () => {

        setIsLoading(true);
        API.get(`List`)
            .then((response) => {
                setList(response.data);
                setIsLoading(false);
            })
            .catch(error => {
                setIsLoading(false);
            });
    }

    useEffect(() => {
        getList();
    }, []);

    const onSubmit = (data) => {

        // format data
        delete data.picture;

        const formData = new FormData();
        formData.append('file', file[0]);
        formData.append('jsonElement', JSON.stringify(data));
        setIsLoading(true);

        API.post(`Upload`, formData, {
            onUploadProgress: progressEvent => {

                setSizeUploaded('Upload Progress :' + Math.round(progressEvent.loaded / progressEvent.total * 100) + '%');
            }
        })
            .then((response) => {

                getList();
                setSizeUploaded('');
            })
            .catch(error => {
                setIsLoading(false);
                setSizeUploaded('');
            });

    };

    const handleDelete = (rowData) => {

        setIsLoading(true);

        API.delete(`Delete/${rowData.name}`)
            .then((response) => {
                console.log('list :', response.data);
                getList();
                setIsLoading(false);
            })
            .catch(error => {
                setIsLoading(false);
            });

    }

    const handleChange = (event) => {
        setFile([event.target.files[0]]);
    };

    const data = list;

    const columns = [
        {
            title: "Photo",
            field: "photo",
            render: (rowData) => <img src={rowData.photo} alt={rowData.name} style={{ width: 40, borderRadius: "50%" }} />,
        },
        { title: "Name", field: "name" },
        { title: "Extension", field: "extension" },
        { title: "Size", field: "length" },
        { title: "Upload Date", field: "uploadDate", type: "numeric" },
    ];

    return (
        <React.Fragment>
            <form onSubmit={handleSubmit(onSubmit)}>

                {/*<input type="text" {...register("name")} />*/}
                <input
                    type="file"
                    {...register("picture")}
                    onChange={handleChange}
                />
                <button type="submit">Submit</button>
                <div>{sizeUploaded}</div>
            </form>
            <br />
            <hr />
            <MaterialTable
                title="File List"
                icons={tableIcons}
                columns={columns}
                data={data}
                isLoading={isLoading}
                actions={[
                    {
                        icon: tableIcons.Delete,
                        tooltip: "Delete File",
                        onClick: (event, rowData) => {
                            handleDelete(rowData);
                        },
                    }
                ]}
            />

        </React.Fragment>
    );
};

export default MyFileUpload;
