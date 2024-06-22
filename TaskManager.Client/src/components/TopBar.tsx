"use client";
import React from "react";
import { AppBar, IconButton, MenuItem, Select, SelectChangeEvent, Toolbar, Typography } from "@mui/material";
import SettingsIcon from '@mui/icons-material/Settings';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';

const TopBar: React.FC = () => {
  const [comboBoxValue, setComboBoxValue] = React.useState('');

  const handleComboBoxChange = (event: SelectChangeEvent<string>) => {
    setComboBoxValue(event.target.value as string);
  };

  return (
    <AppBar position="fixed">
      <Toolbar>
        <Typography variant="h6" noWrap sx={{ flexGrow: 1 }}>
          Task Manager
        </Typography>
        <Select
          value={comboBoxValue}
          onChange={handleComboBoxChange}
          inputProps={{ 'aria-label': 'Without label' }}
          sx={{ color: 'white', borderColor: 'white', '.MuiSvgIcon-root': { color: 'white' } }}
        >
          <MenuItem value={10}>Option 1</MenuItem>
          <MenuItem value={20}>Option 2</MenuItem>
          <MenuItem value={30}>Option 3</MenuItem>
        </Select>
        <IconButton color="inherit">
          <SettingsIcon/>
        </IconButton>
        <IconButton color="inherit">
          <AccountCircleIcon/>
        </IconButton>
      </Toolbar>
    </AppBar>
  );
};

export default TopBar;