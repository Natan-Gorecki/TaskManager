import { Drawer, List, ListItemButton, ListItemText, Toolbar, Box } from '@mui/material';

interface LeftSidebarProps {
  open: boolean;
}

export default function LeftSidebar({ open }: LeftSidebarProps): React.ReactElement {
  return (
    <Drawer variant="persistent" open={open}>
      <Toolbar />
      <Box >
        <List>
          {['Board', 'Tasks', 'Time reports'].map((text) => (
            <ListItemButton key={text}>
              <ListItemText primary={text}/>
            </ListItemButton>
          ))}
        </List>
      </Box>
    </Drawer>
  );
}
