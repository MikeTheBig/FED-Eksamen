import React from "react";

type Option = { value: string; label: string };

interface Props {
  label: string;
  type?: string;
  value?: any;
  onChange?: (e: any) => void;
  placeholder?: string;
  min?: number | string;
  options?: Option[];
  disabled?: boolean;
}

export default function InputField({
  label,
  type = "text",
  value,
  onChange,
  placeholder,
  min,
  options,
  disabled,
}: Props) {
  return (
    <label className="block mb-4">
      <span className="block text-sm font-medium text-gray-700">{label}</span>
      {type === "select" ? (
        <select
          value={value}
          onChange={onChange}
          disabled={disabled}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-200 focus:ring-opacity-50"
        >
          {options && options.map((opt) => (
            <option key={opt.value} value={opt.value}>
              {opt.label}
            </option>
          ))}
        </select>
      ) : type === "textarea" ? (
        <textarea
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          disabled={disabled}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-200 focus:ring-opacity-50"
          rows={4}
        />
      ) : (
        <input
          type={type}
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          min={min}
          disabled={disabled}
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-200 focus:ring-opacity-50"
        />
      )}
    </label>
  );
}
