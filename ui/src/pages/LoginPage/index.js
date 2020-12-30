import { Button, Paper, TextField } from '@material-ui/core';
import React, { useContext } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { useHistory } from 'react-router-dom';
import StoreContext from '../../stores/StoreContext';
import styles from './LoginPage.module.css';

function LoginPage() {
  const { handleSubmit, control, errors } = useForm({ mode: 'onChange' });
  const store = useContext(StoreContext).authenticationStore;
  const history = useHistory();

  const onSubmit = async (data) => {
    await store.loginOrRegister(data.username, data.password);
    history.replace("/task-list");
  }

  return (
    <div className={styles['root']}>
      <Paper className={styles['container']} elevation={3}>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div>
            <Controller
              name="username"
              as={
                <TextField
                  id="username"
                  label="Username"
                  error={!!errors.username}
                  helperText={errors.username && errors.username.message}
                />
              }
              control={control}
              defaultValue=""
              rules={{
                required: "Username is required",
                maxLength: {
                  value: 64,
                  message: "maximum length is 64"
                }
              }}
            />
          </div>
          <div>
            <Controller
              name="password"
              as={
                <TextField
                  id="password"
                  type="password"
                  label="Password"
                  error={!!errors.password}
                  helperText={errors.password && errors.password.message}
                />
              }
              control={control}
              defaultValue=""
              rules={{
                required: "Password is required",
                pattern: {
                  value: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[A-Za-z\d!@#$%^&*()_+=]{8,32}$/,
                  message: "Password must contain 8 - 32 alpanumeric characters with both uppercase and lowercase"
                }
              }}
            />
          </div>
          <div>
            <Button type="submit" variant="contained" color="primary">Login or Register</Button>
          </div>
        </form>
      </Paper>
    </div>
  );
}

export default LoginPage;
