import axios from 'axios';
import { notFound } from '@tanstack/react-router';

export default function throwRouteError(error: unknown) {
  if (axios.isAxiosError(error) && error.response?.status === 404) {
    throw notFound();
  }
  throw error;
}
