import { Button, TextField } from '@material-ui/core';
import React, { useContext } from 'react';
import { Controller, useForm } from 'react-hook-form';
import StoreContext from '../../stores/StoreContext';
import styles from './CreateTaskPage.module.css';

export default function CreateTaskPage() {
  const { handleSubmit, control, errors } = useForm({ mode: 'onChange' });
  const stores = useContext(StoreContext);

  const onSubmit = async (data) => {
    await stores.taskStore.createTask({ title: data.title });
    stores.alertStore.showSuccess("Task has been created");
  }

  return (
    <div className={styles['root']}>
      <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
        <Controller
          name="title"
          as={
            <TextField
              id="title"
              label="Title"
              error={!!errors.title}
              helperText={errors.title && errors.title.message}
            />
          }
          control={control}
          defaultValue=""
          rules={{
            required: "Title is required",
            maxLength: {
              value: 256,
              message: "maximum length is 256"
            }
          }}
        />
        <Button type="submit" variant="contained" color="primary">Create</Button>
      </form>
    </div>
  );
}
